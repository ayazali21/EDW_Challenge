"# EDW_Challenge" 
Tools & Technology Used Here
1. Visual Studio 2019
2. Used .Net Core 3.1


 Steps To Run the Project
 1. Clone this repository
 2. Build the Entire Solution
 3. Make Component Loader as StartUp Project
 4. In Component Loader Project,Open appsetting.json file then change Assembly Directory Path as per your computer location
 5. Manually add FirstAssembly,SecondAssembly.dll to folder name called "Loader" inside ComponentLoader project.
 6. For passing Console command line argument , you can got to properties -> launch setting and add below 
       "commandLineArgs": "-excludeAssembly=^[Second] -excludeClass=^[Second] -instances=2"
 7. Finally Run the ComponentLoader Project.
 
 Note
 1. For any new Assembly library, use attribute called "EDW_ChallengeAttribute"  to work properly.
