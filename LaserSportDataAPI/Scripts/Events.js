function EventsViewModel() {
    var self = this;
    self.events = ko.observableArray();

    var baseUri = '/api/v1/events';
    $.getJSON(baseUri, self.events);
}
jQuery(document).ready(function () {
    ko.applyBindings(new EventsViewModel());
});

function GoToEvent(obj) {
    
    alert('id=' + $(obj).attr('data-id'));
}