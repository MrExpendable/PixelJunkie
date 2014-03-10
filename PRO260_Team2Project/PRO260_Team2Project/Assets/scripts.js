$(document).ready(function() 
{
    var isChecked;
    $('.auctionInfoSection').hide();
    
    $('#isForAuction').click(function()
    {
        isChecked = $(this).is(':checked') ? true : false;
        console.log(isChecked);

        if (isChecked)
        {
            $('.auctionInfoSection').show();
        }
        else
        {
            $('.auctionInfoSection').hide();
        }
    });    
});