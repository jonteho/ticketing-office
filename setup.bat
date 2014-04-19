@Echo off

Echo Initializing database...

sqlcmd -S .\SQLEXPRESS -d TicketingOffice -E -i setup.sql

Echo Initialization completed.

Pause
