var GameRoom = React.createClass({
    getInitialState: function () {
        return {
            inGame: false, //False if user is still in queue waiting for other to join; True if user is engaged 
            currentGame: { //Empty if user not engaged
                cards: [],//{ "Id": 1, "Title": "2016 Mercedes-Benz E300", "ImageUrl": "1.jpg", "CarCharacteristics": [{ "Name": "RRP", "Value": 10 }] }], //List of all cards for this user, for this game (all rounds)
                thisRound: { //Only one user will take a pick per round
                    myTurn: true, //Is this user supposed to make the choice, or wait for other user?
                    myCard: {
                        Id: 1,
                        Active: false,
                        Title: "2016 Mercedes-Benz E300",
                        ImageUrl: "http://d3lp4xedbqa8a5.cloudfront.net/imagegen/max/ccr/300/-/s3/digital-cougar-assets/traderspecs/2016/08/23/Misc/MercedesBenz-E-220-CDI-Sedan-2015-1.jpg",
                        CarCharacteristics: [
                            { Name: "RRP", Value: 10, IsPicked: true },
                            { Name: "GreenHouseRating", Value: 120 }
                        ]
                    },
                    opponentsCard: { //Should be displayed only when user made his pick, else is null
                        Id: 2,
                        Active: false,
                        Title: "2016 Mercedes-Benz E300",
                        ImageUrl: "http://d3lp4xedbqa8a5.cloudfront.net/imagegen/max/ccr/300/-/s3/digital-cougar-assets/traderspecs/2016/08/23/Misc/MercedesBenz-E-220-CDI-Sedan-2015-1.jpg",
                        CarCharacteristics: [
                            { Name: "RRP", Value: 10, IsPicked: true },
                            { Name: "GreenHouseRating", Value: 120 }
                        ]
                    }
                },
                myScore: "",
                opponentsScore: ""
            }
        };
    },
    //componentWillMount: function () {
    //    var that = this;
    //    $.get('/home/getCards').done(function (res) {
    //        // that.setState({ data: data });
    //    });
    //},
    componentDidMount: function () {
        var self = this;
        var vhub = $.connection.gameHub;
        vhub.client.loadGame = function (data) {
            var gameData = JSON.parse(data);
            self.setState(gameData);
        };
        $.connection.hub.start().done(function () {
            vhub.server.joinRoom("xxx");
        });

    },
    render: function () {
        return (
            this.state.inGame
            ?
            <div className="room">
                <div className="table clearfix">
                    <div className="card-wrapper">
                        <h3>Your Card</h3>
                        <Card {...this.state.currentGame.thisRound.myCard} Active={this.state.currentGame.thisRound.myTurn} />
                    </div>
                    <div className="card-wrapper">
                        <h3>Your Opponent's Card</h3>
                        {
                        this.state.currentGame.thisRound.opponentsCard
                        ?
                        <Card {...this.state.currentGame.thisRound.opponentsCard} Active={false} />
                        :
                        <div className="compare-card">?</div>
                        }
                    </div>
                </div>
                <div className="message-bar">{this.state.currentGame.thisRound.myTurn ? 'It is your turn.' : 'Waiting for your opponent to pick.'}</div>
                <div className="status-bar">
                    <span>Wins</span>
                    <span className="status-bar__your-score">3</span>
                    <span className="status-bar__opponent-score">2</span>
                </div>
            </div>
            :
            <div>Waiting for another player to join</div>
        );
    }
});
var Card = React.createClass({
    propTypes: {
        Active: React.PropTypes.bool,
        Title: React.PropTypes.string,
        ImageUrl: React.PropTypes.string,
        CarCharacteristics: React.PropTypes.arrayOf(React.PropTypes.shape({
            Name: React.PropTypes.string.isRequired,
            Value: React.PropTypes.number.isRequired,
            IsPicked: React.PropTypes.bool,
        }))
    },
    onCharacteristicClick: function (characteristicName) {
        console.log(characteristicName);
        // $.post('/home/chooseCharacter').done(function (res) {
            // that.setState({ data: data });
        // });
    },
    render: function () {
        var that = this;
        return (
            <div className="card">
                <div className="card__title">{that.props.Title}</div>
                <div className="card__image"><img src={that.props.ImageUrl} /></div>
                <ul>
                    {
                        that.props.CarCharacteristics.map(function (characteristic) {
                            return that.props.Active
                            ?
                                (
                                    <li className="card__character card__character_active" onClick={that.onCharacteristicClick.bind(that, characteristic.Name)}>
                                        <span className="card__character__name">{characteristic.Name}</span>
                                        <span className="card__character__value">{characteristic.Value}</span>
                                    </li>
                                )
                            :
                                (
                                    <li className="card__character">
                                        <span className="card__character__name">{characteristic.Name}</span>
                                        <span className="card__character__value">{characteristic.Value}</span>
                                    </li>
                                );
                        })
                    }
                </ul>
            </div>
        );
    }
});

ReactDOM.render(
    <GameRoom />,
    document.getElementById('content')
);
