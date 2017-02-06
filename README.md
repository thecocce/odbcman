# odbcman
Windows utility which imports and exports Datasources defined by odbcad32.exe

# Usage
## Listing defined DSNs
* `odbcman --list`
* `odbcman -l`

## Listing available ODBC Drivers
* `odbcman --ListDrivers`
* `odbcman -d`

## Exporting a defined DSN to file 
* `odbcman --export --name [DSN Name] --file [ExportFileName]` 
* `odbcman -e -n [DSN Name] -f [ExportFileName]`

## Importing DSN from file
* `odbcman --import  --file [ExportFileName] [[-name [DSN Name]]`
* `odbcman -i  -f [ExportFileName] [-n [DSN Name]]`

## Remove
* `odbcman --remove --name [DSN Name]`
* `odbcman -x -n [DSN Name]`
