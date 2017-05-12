(function ($, window, ko, undefined) {
    'use strict';

    ko.bindingHandlers.uniqueId = {
        init: function (element) {
            element.id = ko.bindingHandlers.uniqueId.prefix + (++ko.bindingHandlers.uniqueId.counter);
        },
        counter: 0,
        prefix: "unique"
    };

    ko.bindingHandlers.uniqueFor = {
        init: function (element, valueAccessor) {
            var after = ko.bindingHandlers.uniqueId.counter + (ko.utils.unwrapObservable(valueAccessor()) === "after" ? 0 : 1);
            element.setAttribute("for", ko.bindingHandlers.uniqueId.prefix + after);
        }
    };

    var CmAssemblyModel = function (item) {
        var self = this;
        self.Id = ko.bindingHandlers.uniqueId;
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

        self.Components = item.components;
    };

    var CmComponentViewModel = function () {

        var self = this;
        self.assemblies = ko.observableArray([]);
        self.selectedAssemblyDetails = ko.observable();

        self.selectedAssemblyItem = function (value) {
            console.log(value);
            self.selectedAssemblyDetails(value);
            $('input:checkbox[data-toggle="toggle"]').bootstrapToggle();
        };

        self.fetchAssemblies = function () {
            console.log("fetched");
            var jqXHR = $.ajax({
                type: "GET",
                cache: false,
                url: window.cmContext.rootUrl,
                contentType: 'application/json; charset=utf-8',
                success: function (data, textStatus, jqXHR) {
                    self.assemblies(ko.utils.arrayMap(data, function (c) {
                        var model = new CmAssemblyModel(c);
                        self.selectedAssemblyItem(model);
                        return model;
                    }));
                   
                    console.log("success");
                },
                error: function (response) {
                    console.log(response);
                }
            }).always = function (data, textStatus, jqXHR) {
                console.log("always");
            };
        }

        self.initialize = function () {
            self.fetchAssemblies();
        }

        self.initialize();

    };

    ko.applyBindings(new CmComponentViewModel(), document.getElementById("cmContainer"));


})($, window, ko, undefined);