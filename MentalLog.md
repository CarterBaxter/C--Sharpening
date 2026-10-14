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
OccuranceDate,
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

### Log 2 -