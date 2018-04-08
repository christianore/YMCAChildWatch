
DECLARE
	@start		DATETIME = '4-7-2018 9:00',
	@end		DATETIME = '4-7-2018 22:00',
	@interval	INT = 15,
	@partitions  INT = 1,
	@count		INT = 0,
	@tmptime	DATETIME,
	@children	INT,
	@location	INT = 1,
	@minutes INT = 0;

-- Determine the amount of minutes for the difference.
SET @minutes = (DATEPART(HOUR, @end) - DATEPART(HOUR, @start)) * 60
SET @minutes = @minutes + (DATEPART(MINUTE, @end) - DATEPART(MINUTE, @start))
-- Find the partitions
SET @partitions = @minutes / @interval

IF OBJECT_ID(N'tempdb..#interval') IS NOT NULL
	DROP TABLE #interval

CREATE TABLE #interval
(
	id INT ,
	time DATETIME,
	amount INT 
)
-- Create the partitions
WHILE(@count < @partitions)
BEGIN
--Each partition is a factor of the previous amount
SET @tmpTime = DATEADD(MINUTE, @interval * @count, @start);
-- Find the information for people who signed in or out during the partition bounds.
SET @children = (
	SELECT COUNT(child_id) 
	FROM signin_log
	INNER JOIN signin_detail ON
	signin_detail.signin_id = signin_log.signin_id
	WHERE 
		(time_in BETWEEN @tmpTime AND DATEADD(MINUTE, @interval , @tmpTime)
		OR time_out BETWEEN @tmpTime AND DATEADD(MINUTE, @interval , @tmpTime))
		AND watch_location = @location 
)
-- Add this into our list.
INSERT INTO #interval 
VALUES(@count, @tmpTime, @children)
SET @count += 1
END 

SELECT *
FROM #interval



