$(document).ready(function() 
{
    $('#simple-menu').sidr();
    
    $('.imageFilter').click(function()
    {
        $('.filter').slideToggle('fast');
    });
});