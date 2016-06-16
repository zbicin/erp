app.controller('OrderCreateController', function($scope, $http) {
    $scope.order = {
        Id: 0,
        CreatedAt: "",
        CompletedAt: "",
        ShippedAt: "",
        DeliveredAt : "",
        CanceledAt : "",
        SelectedItems: []
    };

    $scope.items = [];

    $scope.LoadItems = function () {
        $http.post("/Orders/GetAvailableItems").then(function (result) {
            $scope.items = result.data;
        });
    }
    $scope.LoadItems();

    $scope.addNewChoice = function ($event) {
        $event.preventDefault();
        var newItemNo = $scope.order.SelectedItems.length + 1 + 1;
        $scope.order.SelectedItems.push({ 'Id': newItemNo });
    };

    $scope.removeChoice = function () {
        var lastItem = $scope.order.SelectedItems.length - 1;
        $scope.order.SelectedItems.splice(lastItem);
    };

    $scope.sendForm = function () {
        $http.post("/Orders/Create", { viewModel: $scope.order }).then(function(response) {
            if (response.data === "OK") {
                window.location.href = '/Orders/Index';
            }
            else {
                alert(response.data);    
            }
        }); 
    };
});