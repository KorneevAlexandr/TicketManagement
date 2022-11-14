CREATE PROCEDURE dbo.DeleteEvent
	@id	INT

AS 

-- deleting event deletes all places related to this event
-- (simulate cascade deletion)

BEGIN
DELETE FROM EventSeat 
WHERE EventAreaId IN (SELECT Id FROM EventArea WHERE EventId = @id)
END

BEGIN
DELETE FROM EventArea
WHERE EventId = @id
END

BEGIN
DELETE FROM Event WHERE Id = @id
END
