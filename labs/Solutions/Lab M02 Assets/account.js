function sayHello() {
    customAlert("Hello Dev Workshop attendees");
};

function calculateEstimatedRevenue(executionContext) {
    debugger;
    var formContext = executionContext;  //PrimaryControl
    var name = formContext.getAttribute("name").getValue();

    var execute_pfedyn_GetSumofEstimatedRevenueforAccount_Request = {
        // Parameters
        AccountName: name, // Edm.String

        getMetadata: function () {
            return {
                boundParameter: null,
                parameterTypes: {
                    AccountName: { typeName: "Edm.String", structuralProperty: 1 }
                },
                operationType: 0, operationName: "pfedyn_GetSumofEstimatedRevenueforAccount"
            };
        }
    };

    Xrm.WebApi.online.execute(execute_pfedyn_GetSumofEstimatedRevenueforAccount_Request).then(
        function success(response) {
            if (response.ok) { return response.json(); }
        }
    ).then(function (responseBody) {
        var result = responseBody;
        console.log(result);
        // Output Parameters
        var sumofestimatedrevenue = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(result["SumofEstimatedRevenue"]);
        customAlert("Estimated revenue for account: " + sumofestimatedrevenue);
    }).catch(function (error) {
        customAlert(error.message);
    });
};

function customAlert(message) {
    try {
        var alertStrings = { confirmButtonLabel: "Ok", text: message };
        var alertOptions = { height: 120, width: 260 };
        Xrm.Navigation.openAlertDialog(alertStrings, alertOptions).then(function () { });
    } catch (err) {
        customAlert(err.message);
    }
};