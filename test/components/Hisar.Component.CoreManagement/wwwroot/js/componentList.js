(function ($, window, ko, undefined) {
    'use strict';

    var CmAssemblyModel = function (item) {
        var self = this;
        self.PackageId = item.packageId;
        self.PackageVersion = item.packageVersion;
        self.Authors = item.authors;
        self.Company = item.company;
        self.Product = item.product;
        self.Description = item.description;
        self.Copyright = item.copyright;
        self.LicenceUrl = item.licenceUrl;
        self.ProjectUrl = item.projectUrl;
        self.IconUrl = item.iconUrl;
        self.RepositoryUrl = item.repositoryUrl;
        self.Tags = item.tags;
        self.ReleaseNotes = item.releaseNotes;
        self.NeutrelLanguage = item.neutrelLanguage;
        self.Version = item.version;
        self.FileVersion = item.fileVersion;

        self.ViewComponents = item.viewComponents;
        self.Controllers = item.controllers;

        self.PackageName = ko.computed(function () {
            var str = self.PackageId ? self.PackageId.toString() : '';
            var nw = str.replace('Hisar.Component.', '');
            console.log(nw);
            return nw;
        }, this);
    };

    var CmComponentViewModel = function () {

        var self = this;
        self.assemblies = ko.observableArray([]);
        self.selectedAssemblyDetails = ko.observable();
        
        self.fetchAssemblies = function () {
            $.ajax({
                type: "GET",
                cache: false,
                url: window.cmContext.componentListRootUrl,
                contentType: 'application/json; charset=utf-8',
                success: function (data, textStatus, jqXHR) {
                    self.assemblies(ko.utils.arrayMap(data, function (c) {
                        return new CmAssemblyModel(c);
                    }));

                    console.log(data);
                },
                error: function (response) {
                    console.log(response);
                }
            }).always = function (data, textStatus, jqXHR) {
            };
        }

        self.initialize = function () {
            self.fetchAssemblies();
        }

        self.initialize();

    };

    ko.applyBindings(new CmComponentViewModel(), document.getElementById("cmListContainer"));


})($, window, ko, undefined);