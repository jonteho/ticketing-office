declare @id int
select @id = id from [ShowsData].[Shows] where Title = 'Othello'

if @id is null
begin
	insert into [ShowsData].[Shows]
	([Title],[Description],[Url],[Preview],[Cast],[Duration],[Logo],[Category])
	values('Othello', 'Othello, the Moor of Venice', 'http://www.imdb.com/title/tt0114057/', 'http://www.imdb.com/title/tt0114057/', '', 0, Null, 'Tragedy')
	
	select @id = @@IDENTITY
end

declare @numOfEvents int
declare @eventID int

select @numOfEvents = COUNT(*) from [EventsData].[Events] where ShowID = @id and date > GETDATE()

if (@numOfEvents = 0)
begin
	set @numOfEvents = 1
	while @numOfEvents <= 4
	begin
		insert into [EventsData].[Events]
		([ID], [ShowID],[TheaterID],[StartTime],[Date],[ListPrice],[State],[PricingPolicy])
		values(NEWID(), @id, (select top 1 ID from TheatersData.Theaters) , getdate(), DateAdd(DAY, @numOfEvents, GETDATE()),200, 'Opened', NULL)

		set @numOfEvents = @numOfEvents + 1
	end
end

Go

