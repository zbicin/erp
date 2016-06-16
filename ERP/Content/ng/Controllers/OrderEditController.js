app.controller('OrderEditController', function ($scope, $http, order) {
    order.SelectedItems = order.SelectedItems == null ? [] : order.SelectedItems;

    $scope.states = [
    { Name: "Created" },
    { Name: "Completed" },
    { Name: "Shipped" },
    { Name: "Delivered" },
    { Name: "Canceled"}
    ];
            
    $scope.order = order;
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

    $scope.sendForm = function() {
        $http.post("/Orders/Edit", { viewModel: $scope.order })
            .success(function() {

                window.location.href = '/Orders/Index';
            });
    };
});