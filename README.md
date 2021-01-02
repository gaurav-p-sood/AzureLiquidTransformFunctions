# Introduction 
This is an Azure Function for Liquid and XSL Transformations without using the Integration Accounts. There are other sample codes as well but I thought of trying it in the most simpler way. 

The function takes a in template parameter in the function URL , retreives the LIQUID/XSL template from a Azure Blob storage and transfroms the body into the expected format.

While making the call to the function , Two request headers are important i.e Content-Type and Accept. The supported values are application/json and application/xml only. 

The soultion targets .NET Core 3.1 

The function runtime is ~3

# Getting Started

Clone the repo and build the code with your project specific changes , Publish it to your azure subscription and its ready !

# Build and Test
The Code can be tested locally via Visual studio and a Postmane or by publishing the function into Azure environment.

In case you wish to try the code , I have hosted it on my APIM instance .

  #Request URL Format 
  https://biztalkers.in/external/TransformFA/V1/template/{templateName}

  #Request URL sample
  https://biztalkers.in/external/TransformFA/V1/template/JsonLiquidMap.liquid

  #Request Body Sample
  POST https://biztalkers.in/external/TransformFA/V1/template/JsonLiquidMap.liquid HTTP/1.1 
  Host: biztalkers.in
  Ocp-Apim-Subscription-Key: abcd
  Accept: application/json
  Content-Type: application/json

  {
    "devices":"laptop , iphone, windows, cloud",
    "firstName":"gaurav",
    "lastName":"sood",
    "phone":"+911234567890"
  }


   #Request URL
   https://biztalkers.in/external/TransformFA/V1/template/JsonLiquidMap.liquid
 
#For More information, Please log on to **https://www.biztalkers.in 

# Contribute
Every is most welcome to make the code better by contirbuting towards it. 

I truly believe in  MAKE IT WORK FIRST  , THEN MAKE IT BETTER.

Regards
Gaurav
