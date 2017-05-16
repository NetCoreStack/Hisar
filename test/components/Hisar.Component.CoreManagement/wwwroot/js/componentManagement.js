function selectText(id) {
    var sel, range;
    var el = document.getElementById(id);
    if (window.getSelection && document.createRange) {
        sel = window.getSelection();
        if (sel.toString() == '') { 
            window.setTimeout(function () {
                range = document.createRange();
                range.selectNodeContents(el);
                sel.removeAllRanges();
                sel.addRange(range);
            }, 1);
        }
    } else if (document.selection) {
        sel = document.selection.createRange();
        if (sel.text == '') { 
            range = document.body.createTextRange();
            range.moveToElementText(el);
            range.select();
        }
    }
}

(function ($, window, ko, hljs, undefined) {
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

        self.ViewComponents = item.viewComponents;
        self.Controllers = item.controllers;
    };

    var CmComponentViewModel = function () {

        var self = this;
        self.assemblies = ko.observableArray([]);
        self.selectedAssemblyDetails = ko.observable();

        self.selectedAssemblyItem = function (value) {
            self.selectedAssemblyDetails(value);
            $('input:checkbox[data-toggle="toggle"]').bootstrapToggle();
            $('code').each(function (i, block) {
                hljs.highlightBlock(block);
            });
            $('.toggleFieldset legend').click(function () {
                $(this).parent().find('legend .fa').toggleClass("fa-plus-square fa-minus-square");
                $(this).parent().find('.toggleFieldsetHiders').toggleClass("hide");
            });
        };

        self.fetchAssemblies = function () {
            $.ajax({
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

                    console.log(data);
                },
                error: function (response) {
                    console.log(response);
                }
            }).always = function (data, textStatus, jqXHR) {
            };
        }

        self.compressComponentList = function() {
            $("#cmSideBar").toggleClass("compressed");
        }

        self.initialize = function () {
            self.fetchAssemblies();
        }

        self.initialize();

    };

    ko.applyBindings(new CmComponentViewModel(), document.getElementById("cmContainer"));


})($, window, ko, hljs, undefined);