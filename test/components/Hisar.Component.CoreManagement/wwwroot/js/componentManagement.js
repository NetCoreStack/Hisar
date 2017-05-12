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

    var initialData = [
        {
            packageId: 'CarouselApp',
            packageVersion: '1.2.3.4',
            authors: 'Gencebay Demir',
            company: 'Bilge Adam Bilişim Hiz. Ltd. Şti.',
            product: 'Carousel',
            description: 'NetcoreStack Hisar Carousel',
            copyright: 'Opengl Licence',
            licenceUrl: 'http://opengl.com',
            projectUrl: 'https://github.com/NetCoreStack/Hisar',
            iconUrl: 'https://cdn0.iconfinder.com/data/icons/summer-2/128/Summer_512px-04.png',
            repositoryUrl: 'https://github.com/NetCoreStack/Hisar',
            tags: 'Hisar,Carousel,Slider,Plugin',
            releaseNotes: '<section><h2>Your placeholder text :</h2><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ad rem redeamus; Sumenda potius quam expetenda. <mark>Maximus dolor, inquit, brevis est.</mark> Duo Reges: constructio interrete.</p><ol><li>Atque haec ita iustitiae propria sunt, ut sint virtutum reliquarum communia.</li><li>Hoc simile tandem est?</li><li>Ad eas enim res ab Epicuro praecepta dantur.</li><li>Ut in voluptate sit, qui epuletur, in dolore, qui torqueatur.</li></ol><pre> 	 Quae quod Aristoni et Pyrrhoni omnino visa sunt pro nihilo, 	 ut inter optime valere et gravissime aegrotare nihil prorsus 	 dicerent interesse, recte iam pridem contra eos desitum est 	 disputari. 	 Contineo me ab exemplis. 	 </pre></section>',
            neutrelLanguage: 'Tuskish,English',
            version: '2.0.0.0',
            fileVersion: '3.0.0.0',
            components: [{ name: 'Carousel' }, { name: 'Sample' }]
        },
        {
            packageId: 'ComponentManagementApp',
            packageVersion: '1.2.3.4',
            authors: 'Taha İPEK',
            company: 'Bilge Adam Bilişim Hiz. Ltd. Şti.',
            product: 'Component Management',
            description: 'NetcoreStack Hisar Component Management',
            copyright: 'Opengl Licence',
            licenceUrl: 'http://opengl.com',
            projectUrl: 'https://github.com/NetCoreStack/Hisar',
            iconUrl: 'https://cdn0.iconfinder.com/data/icons/summer-2/128/Summer_512px-04.png',
            repositoryUrl: 'https://github.com/NetCoreStack/Hisar',
            tags: 'Hisar,Component,Management,Plugin',
            releaseNotes: '<section><h2>Your placeholder text :</h2><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ad rem redeamus; Sumenda potius quam expetenda. <mark>Maximus dolor, inquit, brevis est.</mark> Duo Reges: constructio interrete.</p><ol><li>Atque haec ita iustitiae propria sunt, ut sint virtutum reliquarum communia.</li><li>Hoc simile tandem est?</li><li>Ad eas enim res ab Epicuro praecepta dantur.</li><li>Ut in voluptate sit, qui epuletur, in dolore, qui torqueatur.</li></ol><pre> 	 Quae quod Aristoni et Pyrrhoni omnino visa sunt pro nihilo, 	 ut inter optime valere et gravissime aegrotare nihil prorsus 	 dicerent interesse, recte iam pridem contra eos desitum est 	 disputari. 	 Contineo me ab exemplis. 	 </pre></section>',
            neutrelLanguage: 'Tuskish,English',
            version: '2.0.0.0',
            fileVersion: '3.0.0.0',
            components: [{ name: 'Component' }]
        },
        {
            packageId: 'GuidelineApp',
            packageVersion: '1.2.3.4',
            authors: 'Gencebay Demir',
            company: 'Bilge Adam Bilişim Hiz. Ltd. Şti.',
            product: 'Carousel',
            description: 'NetcoreStack Hisar Guideline',
            copyright: 'Opengl Licence',
            licenceUrl: 'http://opengl.com',
            projectUrl: 'https://github.com/NetCoreStack/Hisar',
            iconUrl: 'https://cdn0.iconfinder.com/data/icons/tutor-icon-set/512/set_of_three_books-128.png',
            repositoryUrl: 'https://github.com/NetCoreStack/Hisar',
            tags: 'Hisar,Guideline,Plugin',
            releaseNotes: '<section><h2>Your placeholder text :</h2><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ad rem redeamus; Sumenda potius quam expetenda. <mark>Maximus dolor, inquit, brevis est.</mark> Duo Reges: constructio interrete.</p><ol><li>Atque haec ita iustitiae propria sunt, ut sint virtutum reliquarum communia.</li><li>Hoc simile tandem est?</li><li>Ad eas enim res ab Epicuro praecepta dantur.</li><li>Ut in voluptate sit, qui epuletur, in dolore, qui torqueatur.</li></ol><pre> 	 Quae quod Aristoni et Pyrrhoni omnino visa sunt pro nihilo, 	 ut inter optime valere et gravissime aegrotare nihil prorsus 	 dicerent interesse, recte iam pridem contra eos desitum est 	 disputari. 	 Contineo me ab exemplis. 	 </pre></section>',
            neutrelLanguage: 'Tuskish,English',
            version: '2.0.0.0',
            fileVersion: '3.0.0.0',
            components: [{ name: 'Guideline' }]
        },
    ];

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

    var CmComponentViewModel = function (datas) {

        var self = this;
        self.assemblies = ko.observableArray([]);

        self.selectedAssemblyItem = ko.observable();

        self.fetchAssemblies = function () {
            console.log("fetched");
            var jqXHR = $.ajax({
                type: "GET",
                cache: false,
                url: window.cmContext.rootUrl,
                contentType: 'application/json; charset=utf-8',
                success: function (data, textStatus, jqXHR) {
                    self.assemblies(ko.utils.arrayMap(data, function (c) {
                        return new CmAssemblyModel(c);
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

    ko.applyBindings(new CmComponentViewModel(initialData), document.getElementById("cmContainer"));


})($, window, ko, undefined);