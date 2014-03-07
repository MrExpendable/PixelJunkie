$(document).ready(function() 
{
    var isChecked;
    $('.priceSection').hide();
    
    $('#isForSale').click(function()
    {
        isChecked = $(this).is(':checked') ? true : false;
        console.log(isChecked);

        if (isChecked)
        {
            $('.priceSection').show();
        }
        else
        {
            $('.priceSection').hide();
        }
    });    
});