# EVE - BACK END
Excel Viewer Editor is an application used to edit excel files in an easy to use web based environment.

## Branching
For the project it is important to work in different branches. Branches are split up like so:

- main
- dev
- testing
- feature/ (This is used for new features that are being worked on)

for the name of what is being worked on kebab-case shall be used.
An example of a branch following these rules is `feature/homepage`

## Pull Requests
No branch is allowed to be merged with the main without a code review by someone who had little to none to do with the code written in that branch.

You can open a pull request whenever, when the work for a branch is done you create or update the pull request and a peer can start a review of this branch. 

After the branch is reviewed you can merge the branch with the dev in consultation with other members of the project.

After testing the dev branch can be merged with the main branch.

## Code structure
Classes that contain data acces shall be named repositories.

The names of fields will start with an _

There will be a folder structure to maintain a clean solution.
DTO's will be in a DTO folder and Controllers inside of a controller folder etc.

We will be using request and response DTO's. The data from a request can be different from a response that you receive from the database.
So the data you receive from an API request will go through the logic in a request DTO.
The data you get from the database will be sent back with a response DTO.

Methods will always start with a capital letter.
