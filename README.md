# odbcman
Windows utility which imports and exports Datasources defined by odbcad32.exe

# Usage
## Register odbcman to extension of export Files
* `odbcman --register`
* `odbcman --r`

## Exporting a defined DSN to file 
* `odbcman --export --name [DSN Name] --file [ExportFileName]` 
* `odbcman -e -n [DSN Name] -f [ExportFileName]`

## Importing DSN from file
* `odbcman --import  --file [ExportFileName]`
* `odbcman -i  -f [ExportFileName]`

## Listing available Drivers
* `odbcman --ListDrivers`
* `odbcman -d`

## Listing defined DSNs
* `odbcman --list`
* `odbcman -l`

## Remove DSN
* `odbcman --remove --name [DSN Name]`
* `odbcman -x -n [DSN Name]`


## Usage Help
* `odbcman --help`
* `odbcman -h`
