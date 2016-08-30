var CarTile = React.createClass({
    handleClick: function () {
        this.props.onChoiceSelect(this.props.choice);
    },
    render: function () {
        var disable = this.props.disable;
        var classString = !disable ? "radio" : "radio disabled";
        return (
            <div className={classString}>
                
            </div>
        );
    }
});


var GameContainer = React.createClass({
    getInitialState: function () {
        return {
            inGame: false, //False if user is still in queue waiting for other to join; True if user is engaged 
            currentGame: { //Empty if user not engaged
                cards: [], //List of all cards for this user, for this game (all rounds)
                thisRound: { //Only one user will take a pick per round
                    myTurn: false, //Is this user supposed to make the choice, or wait for other user?
                    myCard: { //Card active/visible in this round
                        id: "", //Id of the card/car
                        title: "", 
                        imgUrl: "", //Url to image of the car
                        features: [] //  {feature: "", value: ""}
                    }, 
                    opponentsCard: {
                        showCard: false, //Should be displayed only when user made his pick
                        feature: "", //Name of the feature user has picked
                        value: "" //Value of the picked feature
                    }
                }
            }
        };
    },
    componentDidMount: function () {
        $.ajax({
            url: this.props.url,
            dataType: 'json',
            success: function (data) {
                this.setState({
                    //Set state
                });
            }.bind(this),
            error: function (xhr, status, err) {
                console.error(this.props.url, status, err.toString());
            }.bind(this)
        });
    },
    selectedAnswer: function (option) {
        
    },
    handleSubmit: function () {

        var selectedChoice = this.state.user_choice;
        var vhub = $.connection.votingHub;
        $.connection.hub.start().done(function () {
            // Call the Send method on the hub.
            vhub.server.send(selectedChoice);
            // Clear text box and reset focus for next comment.
        });
        this.setState({ is_done: true });
    },
    render: function () {
        var self = this;

        
            var choices = this.state.current_quiz.choices.map(function (choice, index) {
                return (
                    <CarTile />
                );
            });
            var button_name = "Submit";
            return (
                <div className="gameContainer">
                    <h1>Quiz</h1>
                    <p>{this.state.current_quiz.question}</p>
                    {choices}
                    <button id="submit" className="btn btn-default" onClick={this.handleSubmit}>{button_name}</button>
                </div>
            );
        
    }
});

React.render(
    <GameContainer url="/home/getmycards" />,
    document.getElementById('container')
);