# Lab 6 Time Series Insights로 Visualize

## Time Series Insights(TSI) 만들기 

1. The commands below will create a storage account (needed by TSI) and provision the TSI environment

    ```azurecli
    $storage="adtholtsitorage"+(get-random -maximum 10000)
    $tsiname=$random+"tsienv"
    az storage account create -g $rgname -n $storage --https-only -l $location
    $key=$(az storage account keys list -g $rgname -n $storage --query [0].value --output tsv)
    az timeseriesinsights environment longterm create -g $rgname -n $tsiname --location $location --sku-name L1 --sku-capacity 1 --data-retention 7 --time-series-id-properties '$dtId' --storage-account-name $storage --storage-management-key $key -l $location
    ```

1. After the TSI environment is provisioned, we need to setup an event source. We will use the Event Hub that receives the processed Twin Change events

    ```azurecli
    $es_resource_id=$(az eventhubs eventhub show -n tsi-event-hub -g $rgname --namespace $ehnamespace --query id -o tsv)
    $shared_access_key=$(az eventhubs namespace authorization-rule keys list -g $rgname --namespace-name $ehnamespace -n RootManageSharedAccessKey --query primaryKey --output tsv)
    az timeseriesinsights event-source eventhub create -g $rgname --environment-name $tsiname -n tsieh --key-name RootManageSharedAccessKey --shared-access-key $shared_access_key --event-source-resource-id $es_resource_id --consumer-group-name '$Default' -l $location
    ```

1. Finally, configure permissions to access the data in the TSI environment.

    ```azurecli
    $id=$(az ad user show --id $username --query objectId -o tsv)
    az timeseriesinsights access-policy create -g $rgname --environment-name $tsiname -n access1 --principal-object-id $id  --description "some description" --roles Contributor Reader
    ```

## TSI 데이터 보기 

Now, data should be flowing into your Time Series Insights instance, ready to be analyzed. Follow the steps below to explore the data coming in.

1. Open your instance of [Time Series Insights](https://ms.portal.azure.com/#blade/HubsExtension/BrowseResourceBlade/resourceType/Microsoft.TimeSeriesInsights%2Fenvironments) in the Azure portal
1. Click on Go to TSI Explorer at the top of the page.
  ![TSI Environment](./images/tsi-go-to-explorer.png)
1. In the explorer, you will see one Twin from Azure Digital Twins shown on the left. Select GrindingStep, select Chasis Temperature, and hit add.
    ![TSI Explorer](images/tsi-plot-data.png)

    >[!TIP] If you don't see data:
    >
    > - make sure the simulated client is running:
    > - Check for errors in the  
    > - Check for errors in the 

1. You should now be seeing the Chasis Temperature readings from a device named GrindingStep, as shown below.
![TSI Explorer](images/tsi-data.png)

## Challenge: Update Status of Production Line

In the current scenario, the status of the production line is a property in the digital twin.  Think about how the status should be determined and updated using what you've learned.
![Production Line Status](images/challange-prod-line-status.png)