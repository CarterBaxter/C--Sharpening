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

### Log 3 - decoupling via interfaces 

So I left to eat and came back with new ideas of where I should
probably go next. I want to decouple and decrease how much that
controller is currently doing, so I am going to make an interface
for collections of claims and collections of line items.

Im still going to keep the dummy data hardcoded for now just so
something is in the claims collections by default and I dont have 
to create it every time. I dont know if I'll get to seperating it
into an actual database because I want to get to the testing suite
so it might just stay as is now that its seperated and the interface
keeps the app itself from being dependent on how that data is stored

Im adding the calculate total to the line items themselves so aggregateing in the LineItemsCollection isn't as dependent

Sweet That feels alot cleaner and now the calculate total cost is
working. I dont love that I have to go into the claims line items
and then call calculate total cost and think maybe its better
practice to have a function in claim that calls that on lineItems
for it, but for now this is fine.

### Log 4 - More Details of Individual Claims

I think I am going to just make a new view that will have
another table that shows all of a claims line items and details

I forgot how to redirect to the automatic 404 page so looked that up.
Otherwise this has been smooth sailing. Idk Im trying to write down my
thoughts but It's hard to slow down at times and do that especially when 
I don't feel like I'm doing anything novel for this project, I already
kinda talked through making a view and stuff.

Everything is a table and for now thats fine. If I wanted it to look nice
I would be putting in that effort but I'd rather get functionality down
so I can move to testing

This is probably my last bit for tonight but tomorrow I should be able to
finish the functionality and then start the testing.


### Log 5 - Alright new day lets crunch this out

I want to sit down and get the rest going. I am first going to review
the requirements to refocus what I wanted to get done functionally before
I start the testing. My interview is tomorrow morning so tonight is the night
of study.

I need a way to filter by status and loss type on claims

I need claim creation, editing, deletion interfacing and logic
this also goes with the lineitem creation and editing

I need to add the tax calculations for materials and add deductibles
to claims (I could add them to a policy obj but that might be scope creep)
then show the net payable (kinda regretting forgetting these earlier)

Then I need a stauts transition management and then also should fix up the
requirements/code to align for what the statuses are named. Full disclosure
I had AI generate the requirements from my concept that way I was working 
like I would if a client gave me the requirements. However I should have
checked it for consistency before getting into it.

I am considering having ai finish the rest so I can get to testing but we
will see how long this takes me. That could also have the added benefit of
allowing me to simulate the work environment where I am testing code I didn't
write myself, but I'll be playing it by ear.

### Log 6 - Filtering & Tax

I am going to add a filter function for filtering by status and loss type
then I am going to fix the total cost calculation to include tax

Filtering isnt too bad hardest part was figuring out the cleanest way
to get the selected values to persist. Not too bad. 

But I guess I am going to add more cost logic in the claims collection
service like I thought I should've earlier. Also I think the requirement
where it said the estimate calculations should be in a service class is
wrong because I think it belongs in the model but what do I know Im sure
it could work there too. I guess I'm still unsure whether I should have
kept my models bare without methods or if keeping the methods are fine.
I'll look up best practice later if there is one other than preference.

### Log 7 - Status Changes and Requirement Finishing

I think in the respect to my time as I want to have time to cook dinner
tonight I am going to do the status changes myself because I know there
are some inconsistencies there with the requirements and naming but 
then I'll do the creation, editing, and deleting of claims and line items but then I'm going to have AI spot check me and fix up things I run through.