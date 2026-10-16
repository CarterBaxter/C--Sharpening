# Test Plan

## Purpose

This test plan is for TestApp1 and outlines the test cases that will be
created. The tests are initially derived from the app
[requirements](./Requirements.md), and more will be added afterward to
protect against regressions as more code is added. (Even though that likely won't happen in this case because this is a practice app for interview prep.)

## Scope

- In scope: claim and line item rules, estimate calculations, status
  workflow, validation, and the MVC pages.
- Out of scope: performance, security, accessibility, and browsers other
  than Chromium.

## Tools

- Unit: NUnit
- Integration: NUnit + `WebApplicationFactory` (Moq for controller-only tests)
- End-to-end: Playwright for .NET with Page Objects

## Approach

Each rule is tested at the lowest layer that can catch it. The logic is
covered by unit tests, the wiring (controllers actually enforcing the
rule) by integration tests, and E2E is generally just things that need
to be in a browser.
Each test sets up its own data and doesn't depend on any other test.

Priority:
Main priorty is money and the flow of claims, then everything else is secondary.

## Risks / Open Questions

- Rounding mode is unspecified. `Math.Round` defaults to banker's
  rounding (halves go to the even digit), so 2.345 rounds to 2.34, not
  2.35. Need to confirm which rounding the business expects.
- The models accept negative values. Only the form validation rejects negative costs. Anything that skips the form (future API, imports) could create negative totals.
- Tests depend on `DateTime.Today` (date of loss validation, filed date).
- Data is in memory and resets when the app restarts, because havent
 set up a proper database.
- risking missing something important and failing the interview and being sad

---

## Unit Tests

### Reqs Status workflow 1 & 3 (Priority 1 or P1) - `Claim.TryTransitionTo`
Parameterized over every From -> To pair:

| From \ To | None | Open | InReview | Approved | Denied |
|---|---|---|---|---|---|
| None | ✗ | ✓ | ✗ | ✗ | ✗ |
| Open | ✗ | ✗ | ✓* | ✗ | ✗ |
| InReview | ✗ | ✗ | ✗ | ✓ | ✓ |
| Approved | ✗ | ✗ | ✗ | ✗ | ✗ |
| Denied | ✗ | ✗ | ✗ | ✗ | ✗ |

\* only with at least one line item (see workflow 2)

 - ✓ pairs return true and the status changes to the target
 - ✗ pairs return false, the status is unchanged, and the error explains
   the allowed path
 - Same-status moves (Open -> Open) are rejected
 - Approved and Denied are final, so nothing moves out of them
 - An undefined status value, e.g. `(StatusType)99`, is rejected

### Reqs Status workflow 2 (P1)
 - Open -> InReview with 0 line items returns false with the error
   "A claim needs at least one line item before it can move to InReview."
 - Open -> InReview with 1 line item succeeds
 - Open -> InReview with 3 line items succeeds

### Reqs Claims 4, Line items 2, Status workflow 4 (P1) - `Claim.CanEdit`
 - None -> true
 - Open -> true
 - InReview -> false
 - Approved -> false
 - Denied -> false

### Reqs Claims 6 (P1) - `Claim.CanDelete`
 - Open -> true
 - None, InReview, Approved, Denied -> false

### Reqs Line items 3 (P1) - `LineItem.TotalCost`
 - 10.00 × 3 -> 30.00
 - 19.99 × 2.5 -> 49.975 -> 49.98
 - 0.00 × 5 -> 0.00
 - Rounding question: 2.345 × 1 -> actual 2.34 (banker's rounding);
   2.35 if away-from-zero is expected
 - Rounding question: 0.335 × 3 = 1.005 -> actual 1.00; 1.01 if
   away-from-zero is expected

### Reqs Estimate calculation 2 (P1) - `LineItem.TaxAmount`
 - Materials, 100.00 × 1 -> tax 6.50
 - Labor, 100.00 × 1 -> tax 0.00
 - Equipment and PersonalProperty, 100.00 × 1 -> tax 0.00
 - Materials, 0.00 × 1 -> tax 0.00
 - Rounding question: Materials, 1.00 × 1 -> 0.065 -> actual 0.06
   (banker's rounding); 0.07 if away-from-zero is expected

### Reqs Estimate calculation 1 & 3 (P1) - `LineItemsCollection` / `Claim`
 - No line items -> subtotal 0.00, tax 0.00, gross 0.00
 - Materials 100.00 + Labor 50.00 -> subtotal 150.00, tax 6.50, gross 156.50
 - Two Materials items, 100.00 and 200.00 -> subtotal 300.00, tax 19.50,
   gross 319.50
 - `Claim.CalculateTotalClaimCost` equals the collection's gross total

### Reqs Validation 1 & 2 (P1) - `ClaimFormViewModel` / `NotInFutureAttribute`
Using `Validator.TryValidateObject` on a valid form, changing one field per test:
 - Date of loss = today -> valid
 - Date of loss = yesterday -> valid
 - Date of loss = tomorrow -> error "Date of loss can't be in the future."
 - Date of loss empty -> error "Date of loss is required."
 - Loss type None -> error "Choose a loss type."
 - Policyholder name, address or description empty -> required error
 - Policy number 0 -> error "Policy number must be a positive number."
 - All fields valid -> no errors

### Reqs Validation 1 (P2) - `LineItemFormViewModel`
 - Cost per unit -0.01 -> error; 0.00 -> valid
 - Units 0 -> error; 0.01 -> valid
 - Category None -> error "Choose a category."
 - Description empty -> required error

### Reqs Claims 2 (P2) - `ClaimsCollection.GetClaims`
 - No filters -> all claims
 - Status = Open -> only Open claims
 - Loss type = Fire -> only Fire claims
 - Status = Open + loss type = Fire -> only claims matching both
 - A filter with no matches -> empty list

### Reqs Claims 3 (P2) - `ClaimsCollection.AddClaim` / `LineItemsCollection.AddLineItem`
 - A new claim gets max existing ID + 1
 - Adding to an empty collection gives ID 1
 - Same two cases for line item IDs

---

## Integration Tests

Using `WebApplicationFactory` with real HTTP requests (including the
antiforgery token).

### Reqs Claims 1 (P2)
 - GET `/Claims` returns 200 and shows claim number, policyholder, loss
   type, status and estimate total for each claim

### Reqs Claims 2 (P2)
 - GET `/Claims?status=4` returns only Denied claims and Denied is
   selected in the dropdown
 - GET `/Claims?status=&lossType=` returns all claims

### Reqs Claims 3 (P1)
 - A valid POST to `/Claims/Create` redirects to the new claim's details
 - The new claim has a generated claim number, status Open, and FiledDate
   of today

### Reqs Claims 4 (P1)
 - A valid edit POST on an Open claim saves and redirects
 - An edit POST on an InReview / Approved / Denied claim is rejected and
   the claim is unchanged
 - Posting `Status=Approved` with an edit form does not change the status
   (overposting)

### Reqs Claims 5 (P2)
 - GET `/Claims/Details/{id}` shows all line items, subtotal, tax and
   gross total
 - GET `/Claims/Details/999` returns 404

### Reqs Claims 6 (P1)
 - A delete POST on an Open claim removes it (details then returns 404)
 - A delete POST on a non-Open claim is rejected with "Only Open claims
   can be deleted." and the claim still exists

### Reqs Line items 1 (P1)
 - Add, edit and remove line items on an Open claim, with totals updating

### Reqs Line items 2 (P1)
 - Add, edit and remove POSTs on an InReview / Approved / Denied claim
   are rejected and the line items are unchanged

### Reqs Status workflow 1–3 (P1)
 - A status change POST with a valid move updates the status
 - A status change POST with an invalid move shows the error and the
   status is unchanged

### Reqs Validation 1 (P1)
 - An invalid create POST returns the form (200, not a redirect) with each
   error message next to its field, and no claim is created

### Security (P2)
 - POSTs without an antiforgery token return 400

---

## End-to-End Tests

Each flow starts from a fresh app and creates its own data.

1. Create a claim (Reqs Claims 3, 5): fill in the form, submit, and
   see the details page with status Open. Back on the list, the claim
   appears.
2. Validation messages (Reqs Validation 1, 2): submit the create form
   with a future date and blank fields, and see each message next to its
   field.
3. Filter the list (Reqs Claims 2): filter by Fire and see only Fire
   claims; filter by Water and the Fire claim is gone; clear the filters
   and everything shows.
4. Workflow (Reqs Status workflow 2, 3, Line items 1): try to move a
   new claim to InReview and see the error; add a line item; move it to
   InReview successfully.
5. Locked claim (Reqs Claims 4, Line items 2): a claim in InReview has
   no Edit, Delete, Add Line Item or line item edit/remove buttons; move it
   to Approved and it stays locked.
