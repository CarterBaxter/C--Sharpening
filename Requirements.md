# Functional requirements

## Claims (MVC pages)

1. A user can view a list of all claims showing claim number, policyholder, loss type, status and estimate total.
2. The list can be filtered by status and loss type.
3. A user can create a claim. The claim number is generated automatically and the status starts as Open.
4. A user can edit claim details while the claim is not InReview.
5. A user can view claim details, including all line items and the calculated totals.
6. A user can delete a claim only while it is Open.

## Line items

1. A user can add, edit and remove line items on a claim.
2. Line items can’t be changed once the claim is InReview.
3. Each line item’s total is Quantity × UnitCost, rounded to 2 decimal places.

## Estimate calculation

1. The subtotal is the sum of all line item totals.
2. Tax is 6.5% of the Materials line items only. This creates a nice edge case to test.
3. The gross total is subtotal plus tax.

## Status workflow

1. Valid transitions are None → Open → InReview → Approved || Denied.
2. A claim can’t move to InReview unless it has at least one line item.
3. Skipping steps or moving backward is rejected with a clear error message.
4. A claim and its line items cant be edited once the status is InReview

## Validation

1. A future DateOfLoss is rejected.
2. A future FiledDate is rejected.