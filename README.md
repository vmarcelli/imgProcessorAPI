# Image Processor API

This is the REST API example project from the first night of class. The following explain
a few useful things in case you want to get stuff running.

## How to get it to work

* Copy the picture you'd like to edit to the ImageProcessorAPI directory
* Use the /Image POST call to upload the picture
* Edit the image with any number of Image/picturebox calls you would like
* Download the image to the ImageProcessorAPI directory using the /Image GET call
* All done!

## To build the application

    dotnet publish

## To build and run the application locally

    dotnet restore
    dotnet build
    dotnet run   

## Notes

* This API was not built to stop bad image editing decisions. If you attempt to do something such as size a thumbnail to a poster sized image the API will let you do so. The resulting horror of pixelation is your responsibility. 
