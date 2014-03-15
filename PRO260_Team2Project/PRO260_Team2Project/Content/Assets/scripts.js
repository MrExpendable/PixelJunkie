$(document).ready(function () {
    $('#bidInfo').hide();
    var selectionBox = $('#listSelection');
    selectionBox.change(function () {
        if (selectionBox.val() == "BidandBuy") {
            $('#bidInfo').show();
        }
        else {
            $('#bidInfo').hide();
        }
    });
});