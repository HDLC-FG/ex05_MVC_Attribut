$(document).ready(function () {
    var fieldCount = 0;

    //Button to add article
    $('#AddArticle').click(function () {
        fieldCount++;

        var articleSelectedValue = $('#ArticleSelected').find(":selected").val();
        var articleSelectedText = $('#ArticleSelected').find(":selected").text();
        var articleQteSelectedValue = document.getElementById('ArticleQteSelected').value;

        if (articleSelectedValue && articleQteSelectedValue && articleQteSelectedValue > 0) {
            $.ajax({
                url: '/Order/CreateArticle',
                type: 'POST',
                data: {
                    articleSelected: {
                        Id: articleSelectedValue,
                        Name: articleSelectedText,
                        Qte: articleQteSelectedValue,
                        Price: null
                    }
                },
                success: function (response) {
                    console.log("CreateArticle success");
                    if (!response.success) {
                        ArticlesValidationMessage("Not enough stock.");
                    }
                    else {
                        ArticlesValidationMessage("");
                    }
                    document.getElementById("idTotalAmount").value = response.totalAmount;
                    $('#ListArticlesSelectedView').html(response.partialView);
                },
                error: function () {
                    console.log("CreateArticle error");
                }
            });

            ArticlesValidationMessage("");
        }
        else {
            ArticlesValidationMessage("An article and quantity greater than 0 is required.");
        }
    });

    function ArticlesValidationMessage(message) {
        document.getElementById("ArticlesValidationMessage").textContent = message;
    }

    //Button delete article in partial view _ListeArticles.cshtml
    $(document).on('click', '.delete-btn', function () {
        var articleId = $(this).data('id');
        var articleQuantity = $(this).data('qte');

        $.ajax({
            url: '/Order/DeleteArticle',
            type: 'POST',
            data: {
                Id: articleId,
                Qte: articleQuantity
            },
            success: function (response) {
                console.log("DeleteArticle success");

                document.getElementById("idTotalAmount").value = response.totalAmount;
                $('#ListArticlesSelectedView').html(response.partialView);
            },
            error: function () {
                console.log("DeleteArticle error");
            }
        });
    });

    //When OrderStatus change
    $('#OrderStatus').change(function () {
        var selectedValue = $(this).val();
        console.log("selectedValue : " + selectedValue);

        //OrderStatus 1 : InProgress
        if (selectedValue && selectedValue == 1) {
            $('#ArticleSelected').prop('disabled', true);
            $('#ArticleQteSelected').prop('disabled', true);
            $('#AddArticle').prop('disabled', true);
        } else {
            $('#ArticleSelected').prop('disabled', false);
            $('#ArticleQteSelected').prop('disabled', false);
            $('#AddArticle').prop('disabled', false);
        }
    });
});