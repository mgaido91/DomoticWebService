Joseph Garrone 0033636786385 joseph.garrone.GJ@gmail.com


To use this project, you need:

1 - create the DB according to the script (https://www.dropbox.com/sh/5s7pf0ogjgry1y5/AABMqD-12ZvFWxFH5EGedooya?dl=0&preview=db_script.sql)

2 - create a user with full access to the DB created

3 - edit the connection string of the project accordingly to the credentials choosen

4 - compile the whole solution

5 - Install the service by right-clicking on the  Visual Studio Developer command prompt and run it as administrator,
    then execute the command
              InstallUtil.exe -i __YOUR_PATH_TO_THE_RELEASE_FOLDER/DomoticService.exe
              
6 - open the "Services" window and start the service
