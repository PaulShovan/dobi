define(['app', 'underscore'], function (app, _) {
    app.controller('dashboardController', ['$scope', '$localStorage', 'roleConstant', function ($scope, $localStorage, roleConstant) {
        "use strict";

        $scope.Data = {
            UserInfo: {
                Name: "",
                Role: ""
            },
            ViewName: "",
            MenuList: []
        };

        $scope.Menues = {
            SuperAdminMenues: [
                { Title: "Add New Dobi", State: "dobiadd", IsDisabled: false },
                { Title: "Manage Dobi", State: "dobimanage", IsDisabled: false },
                { Title: "Add User", State: "useradd", IsDisabled: false },
                { Title: "Manage User", State: "usermanage", IsDisabled: false },
                { Title: "Add Promo", State: "managepromo", IsDisabled: false },
                { Title: "Payment", State: "dashboard", IsDisabled: true }
            ],
            AdminMenues: [
                { Title: "Add New Dobi", State: "dobiadd", IsDisabled: false },
                { Title: "Manage Dobi", State: "dobimanage", IsDisabled: false },
                { Title: "Add User", State: "useradd", IsDisabled: false },
                { Title: "Manage User", State: "usermanage", IsDisabled: false },
                { Title: "Add Promo", State: "managepromo", IsDisabled: false },
                { Title: "Payment", State: "dashboard", IsDisabled: false }
            ],
            ManagerMenues: [
                { Title: "All Orders", State: "orders", IsDisabled: false },
                { Title: "In Progress", State: "dashboard", IsDisabled: true },
                { Title: "Cleaned Items", State: "dashboard", IsDisabled: true },
                { Title: "Deliverable Items", State: "dashboard", IsDisabled: true },
                { Title: "Delivered", State: "dashboard", IsDisabled: true }
            ]
        }

        $scope.Methods = {
            Init: function () {
                $scope.Data.UserInfo = $localStorage.UserInfo;
                $scope.Data.ViewName = $localStorage.ViewName;
            },
            SelectedMenu: function () {
                _.each($scope.Menues, function (menu) {
                    if ($scope.Data.UserInfo.Role === roleConstant.Superadmin) {
                        $scope.Data.MenuList.push(menu);
                    } else if ($scope.Data.UserInfo.Role === roleConstant.Admin) {
                        
                    }
                });

                if ($scope.Data.UserInfo.Role === roleConstant.Superadmin) {
                    $scope.Menues.MenuList = SuperAdminMenues;
                }
            }
        };

        console.log("Dashboard Controller");
        $scope.Methods.Init();
    }]);

});