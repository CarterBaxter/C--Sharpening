# Functional requirements

## Claims (MVC pages)

1. A user can view a list of all claims showing claim number, policyholder, loss type, status and estimate total.
2. The list can be filtered by status and loss type.
3. A user can create a claim. The claim number is generated automatically and the status starts as New.
4. A user can edit claim details while the claim is not Closed.
5. A user can view claim details, including all line items and the calculated totals.
6. A user can delete a claim only while it is New.

## Line items

1. A user can add, edit and remove line items on a claim.
2. Line items can’t be changed once the claim is Approved or Closed.
3. Each line item’s total is Quantity × UnitCost, rounded to 2 decimal places.

## Estimate calculation (your core business logic, so put it in a service class)

1. The subtotal is the sum of all line item totals.
2. Tax is 6.5% of the Materials line items only. This creates a nice edge case to test.
3. The gross total is subtotal plus tax.
4. The net payable is the gross total minus the deductible, and never goes below $0.00.

## Status workflow

1. Valid transitions are New → Inspected → Estimated → Approved → Closed.
2. A claim can’t move to Estimated unless it has at least one line item.
3. Skipping steps or moving backward is rejected with a clear error message.
4. A claim can go from Estimated back to Inspected (for re-inspection). This is the one allowed backward move, which gives you something interesting to test.

## Validation

1. All validation rules above are enforced on the server and shown as messages next to the relevant fields in the form.
2. A future DateOfLoss is rejected.