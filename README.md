1) Install SyncService
2) In service properties ("log on" tab) choose "log on as this account" and enter correct data (user + password)
3) run Regedt32.exe, go to "Computer\HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services", find SyncService, 
add full control permissions for account that was chosen on step 2.
4) start Configurations server
5) start UI service configurator
6) get Google credantials:
	Step 1 – Open to Google APIs Console using this link- https://console.developers.google.com/apis
	Step 2 – Login to Google API Console using your Google account
	Step 3 – After login, you are redirected to Google APIs Dashboard. 
Here you need to enable Google Calendar API. Click on “ENABLE API AND SERVICES” link to enable it. 
This will redirect you to all Google APIs list page. Here in the search bar, type the “Google Calendar. 
It will filter or search the result as below screenshot. Click on the first result item which is Google Calendar API.
	Step 4 – After selecting the API result this will show a button to enable it hit that button.
	Step 5 – You need to create a project to use or enable any Google APIs. 
It will redirect you to project creation page, where you need to create a project. Click on “Create” button.
	Step 6 – Now, we will generate the API credentials. 
Click on Credentials link or after enabling calendar API you will automatically redirect to the credential page. 
7) create teamup calendar https://www.teamup.com/ 
8) enter google credentials and teamUp calendar key in UI
9) after authorizing set configurations (choose calendars, select timeout, select privacy)
10) after successful configurations' setting you can start service with one start parameter (your username) 
11) enjoy the work of SyncService