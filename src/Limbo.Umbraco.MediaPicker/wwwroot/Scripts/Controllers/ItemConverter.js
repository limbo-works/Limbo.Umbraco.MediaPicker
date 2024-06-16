angular.module("umbraco").controller("Limbo.Umbraco.MediaPicker.ItemConverter.Controller", function ($scope, $http, editorService) {

    // Get the base URL for the API controller
    const baseUrl = Umbraco.Sys.ServerVariables.umbracoSettings.umbracoPath;

    const vm = this;

    vm.loaded = false;
    vm.converters = [];

    // Get the query string of the view URL
    const urlParts = $scope.model.view.split("?");
    const urlQuery = new URLSearchParams(urlParts.length === 1 ? "" : urlParts[1]);

    // Get the "editor" parameter from the query string
    const v = urlQuery.get("v");
    const editor = urlQuery.get("editor");

    vm.reset = function () {
        vm.selected = null;
        $scope.model.value = "";
        delete vm.notFound;
        delete vm.obsolete;
    };

    vm.add = function () {

        editorService.open({
            title: "Select image model",
            size: "medium",
            view: `/App_Plugins/Limbo.Umbraco.MediaPicker/Views/ItemConverterOverlay.html?v=${v}`,
            filter: true,
            availableItems: vm.converters,
            submit: function (model) {
                vm.selected = model;
                $scope.model.value = { type: model.type };
                delete vm.notFound;
                editorService.close();
                update();
            },
            close: function () {
                editorService.close();
            }
        });

    };

    function update() {

        vm.obsolete = null;

        if (!vm.selected) return;

        if (vm.selected.obsolete) {
            if (vm.selected.obsolete.message) {
                vm.obsolete = "The selected item converter has been marked as obsolete: " + vm.selected.obsolete.message;
            } else {
                vm.obsolete = "The selected item converter has been marked as obsolete.";
            }
        }

    }

    function init() {

        if (!$scope.model.value) $scope.model.value = "";

        $http.get(`${baseUrl}/backoffice/Limbo/MediaPicker/GetItemConverters?editor=${editor}`).then(function (response) {

            vm.loaded = true;
            vm.converters = response.data;

            if (!$scope.model.value) return;

            if (typeof $scope.model.value === "string") {
                $scope.model.value = { type: $scope.model.value.split(", Version")[0] };
            }

            vm.selected = vm.converters.find(x => x.type === $scope.model.value.type);

            if (!vm.selected) vm.notFound = true;

            update();

        });

    }

    init();

});