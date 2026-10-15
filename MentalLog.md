# Mental Log

## Description

This is a log of my thoughts while creating this to shed more insight into how
I work and what I am thinking as I work on this project. This may be less formatted
but so is my brain.

## Logs

### Log 1 - Creating Models

So first things first I want to create the classes/models that will be used in
the app. So a claim model which is going to have: 
ClaimId,
PolicyholderName,
PolicyAddress,
PolicyNumber
LossType,
Description,
LossDate, # changed to loss date because easier to spell
FiledDate,
Status,
and LineItems # so the individual items being claimed

Then each LineItem will have: 
ItemId,
ClaimId, # to tie it back to its claim
Description,
Category, # like labor, materials, or equipment
CostPerUnit,
Units

I dont remember if its typical to do enums in their own file or not
Im not gunna worry about it for now.

### Log 2 - Simple Controller and Views Given Static Claims

So I think now I am going to build a simple controller to create
like 2 claims with a few claim items each. These will be hardcoded
just so I can start on a simple view to display them in a table.

Okay so I made the dummy data and the table html but running into
the issue of a null reference exception for model in the for loop
I think Im not passing it right so I'm going to look it up to remember

Also Im just now paying attention to this Simplify new expression (IDE0090)
and I frankly don't know how I feel about it. I think It could make it less readable but on the other hand that might just be my slight resistance to the change.

Ah the error is because Im dumb and just tried to pass it into the
Home index file instead of in the index file on claims... I feel bad for that one lol. But this is why I am sharpening back up.

Okay sweet I got the basic info showing in a table, now I want to
create the Est. Total service so total can be shown on the claim
overview. After that I cant decide if I want to make the editing
and viewing of an individual claim a new page or just drop down or
a overlay frame or something. I need to remember not to get too
fancy tho cuz I wanna get to the testing suite.