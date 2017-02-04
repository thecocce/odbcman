# odbcman
Windows utility which imports and exports Datasources defined by odbcad32.exe

# Usage
## Listing defined DSNs
* `odbcman --list`
* `odbcman -l`
* `odbcman /l`

## Exporting a defined DSN to file 
* `odbcman --export [DSN Name] --file [ExportFileName]` 
* `odbcman -e [DSN Name] -f [ExportFileName]`
* `odbcman /e [DSN Name] /f [ExportFileName]`

## Importing DSN from file
* `odbcman --import  --file [ExportFileName] [-Name [NewName]]`
* `odbcman --i  --file [ExportFileName] [-Name [NewName]]`
* `odbcman /i  /file [ExportFileName] [-Name [NewName]]`

## Remove
* `odbcman --remove [DSN Name]`
* `odbcman --x [DSN Name]`
* `odbcman /x [DSN Name]`
