let playerScore = 0;

new SlimSelect({
    select: '#categoryList',
    settings: {
        searchText: '😞😞 Nothing Found 😞😞',
        searchPlaceholder: 'Choose your trivia topic!'
    }
});

new SlimSelect({
    select: '#difficulty',
    settings: {
        showSearch: false
    }
});
var TriviaBook = (function ()
{
    var config = {
        $bookBlock: $('#triviaBook'),
        $startTrivia: $('#startTriviaBtn'),
        $confirmTriviaConfig: $('#confirmTriviaConfig'),
        $nextQuestion: $('.nextQuestion'),
        $triviaResult: $('#triviaResult')
    },
        init = function ()
        {
            config.$bookBlock.bookblock({
                orientation: 'horizontal',
                speed: 800,
                shadowSides: 0.8,
                shadowFlip: 0.7
            });
            initEvents();
        },
        initEvents = function ()
        {

            var $slides = config.$bookBlock.children();
            config.$startTrivia.on('click', function ()
            {
                config.$bookBlock.bookblock('next');
                return false;
            });
            config.$confirmTriviaConfig.on('click', function ()
            {
                var category = $('#categoryList').val();
                var difficulty = $('#difficulty').val();
                config.$bookBlock.bookblock('next');
                $.ajax({
                    type: 'GET',
                    url: '/Home/InitializeTrivia',
                    data: {
                        category: category,
                        difficulty: difficulty
                    },
                    success: function (data)
                    {
                        $.each(data, function (index, item)
                        {
                            $('#questionCard-' + item.questionNumber).find('.question').text(item.question);

                            $.each(item.multipleChoice, function (index, multipleChoice)
                            {
                                $('#questionCard-' + item.questionNumber).find('.multipleChoice').append('<li class="list-group-item border-0"><button type = "button" id="q-' + item.questionNumber + '_ch-' + index + '" class= "multipleChoiceBtn btn btn-outline-primary text-start w-100 p-3" onClick="CheckAnswer(' + item.questionNumber + ',\'' + multipleChoice.replace(/'/g, "\\'") + '\','+index+')">' + multipleChoice + '</button></li>')
                            });
                        });
                    },
                    error: function ()
                    {
                        alert('Error fetching data.');
                    }
                });
                return false;
            });
            config.$nextQuestion.on('click', function ()
            {
                $(".multipleChoiceBtn").removeClass("animate__animated");
                config.$bookBlock.bookblock('next');
                return false;
            });
            config.$triviaResult.on('click', function ()
            {
                $(".multipleChoiceBtn").removeClass("animate__animated");
                if (playerScore > 15) {
                    $("#totalScore #circleAnimation").addClass("circleAnimationSuccess");
                }
                if (playerScore >= 10 && playerScore < 15) {
                    $("#totalScore #circleAnimation").addClass("circleAnimationAverage");
                }
                if (playerScore < 10) {
                    $("#totalScore #circleAnimation").addClass("circleAnimationFail");
                }
                config.$bookBlock.bookblock('next');
                return false;
            });
        };

    return { init: init };

})();

function CheckAnswer(questionNumber, answerSelected, index)
{
    $.ajax({
        type: 'GET',
        url: '/Home/CheckAnswer',
        data: {
            questionNumber: questionNumber,
            answerSelected: answerSelected
        },
        success: function (data)
        {
            $("#questionCard-" + questionNumber + " .multipleChoiceBtn").prop("disabled", true);
            if (data.isCorrect == true) {
                $("#q-" + questionNumber + "_ch-" + index + "").removeClass("btn-outline-primary").addClass("btn-success animate__animated animate__shakeY");
                playerScore++;
                $('.currentScore').text(playerScore.toString().padStart(2, '0'));
            }
            else {
                $("#q-" + questionNumber + "_ch-" + index + "").removeClass("btn-outline-primary").addClass("btn-danger animate__animated animate__shakeX ");
                $("#footer-" + questionNumber + " .nextQuestion").before('<div class="alert alert-warning text-center align-content-center" role="alert"><p class="m-0">Correct Answer: ' + data.answer + '</p></div>');
            }
            $("#footer-" + questionNumber).removeAttr("hidden");
        },
        error: function ()
        {
            alert('Error fetching data.');
        }
    });

    $('#retryBtn').on("click", function ()
    {
        location.reload(); // Reloads the entire page
    });
};