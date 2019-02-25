--Problem 12. Countries Holding ‘A’ 3 or More Times
Select CountryName, ISOCode
From Countries
WHERE CountryName LIKE '%a%a%a%'
ORDER BY IsoCode

--Problem 13. Mix of Peak and River 
SELECT  Peaks.PeakName ,Rivers.RiverName, LOWER(SUBSTRING(peaks.PeakName, 1 ,LEN(PeakName)-1 ) + RiverName) as Mix
FROM Rivers, Peaks
WHERE SUBSTRING(PeakName, LEN(PeakName) ,1 ) =  SUBSTRING(RiverName, 1,1) 
ORDER BY Mix 