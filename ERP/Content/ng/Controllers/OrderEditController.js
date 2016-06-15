app.controller('OrderEditController', function ($scope, $http, order) {
    order.SelectedItems = order.SelectedItems == null ? [] : order.SelectedItems;

    $scope.order = order;
    $scope.choices = [];
    $scope.items = [];

    $scope.LoadItems = function () {
        $http.post("/Orders/GetAvailableItems").then(function (result) {
            $scope.items = result.data;//.Items;           
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