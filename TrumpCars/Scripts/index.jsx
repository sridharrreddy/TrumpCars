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
                        ],
                        "Win": true
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
            console.log(gameData);
            self.setState(gameData);
        };
        $.connection.hub.start().done(function () {
            vhub.server.joinRoom("xxx");
        });

    },
    onNextRoundClick: function (groupName) {
        var vhub = $.connection.gameHub;
        vhub.server.nextRound(groupName).done(function () {
            console.log('Invocation of NextRound succeeded');
        }).fail(function (error) {
            console.log('Invocation of NextRound failed. Error: ' + error);
        });
        //console.log(characteristicName);
        // $.post('/home/chooseCharacter').done(function (res) {
        // that.setState({ data: data });
        // });
    },
    render: function () {
        var that = this;
        return (
            this.state.inGame
            ?
            <div className="room">
                {
                    this.state.currentGame.isGameFinished
                    ?
                    <div className="lose-pic">
                    {
                        (this.state.currentGame.thisRound.opponentsScore > this.state.currentGame.thisRound.myScore)
                        ?
                        <img src="http://static.businessinsider.com/image/5603216fbd86ef20008bc9c0/image.jpg" />
                        :
                        <img src="http://cdn3.thr.com/sites/default/files/2015/10/donald_trump.jpg" />
                    }
                    </div>
                    :
                    <div className="table-wrapper">
                        <div className="table clearfix">
                            <div className="divider">VS</div>
                            <div className="card-wrapper">
                                <h3>Your Card</h3>
                                <Card {...this.state.currentGame.thisRound.myCard} Active={that.state.currentGame.thisRound.myTurn} GroupName={that.state.roomName} />
                            </div>
                            <div className="card-wrapper">
                                <h3>Your Opponent's Card</h3>
                                {
                                    this.state.currentGame.thisRound.opponentsCard != null
                                    ?
                                    <Card {...this.state.currentGame.thisRound.opponentsCard} Active={false} />
                                    :
                                        <div className="compare-card">?</div>
                                }
                            </div>
                        </div>
                        <div className="message-bar">
                            {
                                this.state.currentGame.thisRound.opponentsCard != null
                                ?
                                (
                                    this.state.currentGame.isGameFinished
                                    ? "Game Finished"
                                    : <a href="#" onClick={this.onNextRoundClick.bind(this, this.state.roomName) }>Next Round</a>
                                )
                                :
                                (
                                    this.state.currentGame.thisRound.myTurn
                                    ? 'It is your turn.'
                                    : 'Waiting for your opponent to pick.'
                                )
                            }
                        </div>
                    </div>
                }
                <div className="status-bar">
                    <span>Wins</span>
                    <span className="status-bar__score status-bar__score_your">{this.state.currentGame.thisRound.myScore}</span>
                    <span className="status-bar__counter">{this.state.currentGame.counter == 0 ? 'Finished' : this.state.currentGame.counter + 'rounds to go'}</span>
                    <span>Loses</span>
                    <span className="status-bar__score status-bar__score_opponent">{this.state.currentGame.thisRound.opponentsScore}</span>
                </div>
            </div>
            :
            <div className="waiting-to-start">
                <div className="spinner"></div>
                Waiting for another player to join
            </div>
        );
    }
});
var Card = React.createClass({
    propTypes: {
        Id: React.PropTypes.number.isRequired,
        GroupName: React.PropTypes.string,
        Result: React.PropTypes.string,
        Active: React.PropTypes.bool,
        Title: React.PropTypes.string,
        ImageUrl: React.PropTypes.string,
        CarCharacteristics: React.PropTypes.arrayOf(React.PropTypes.shape({
            Name: React.PropTypes.string.isRequired,
            Value: React.PropTypes.number.isRequired,
            Picked: React.PropTypes.bool
        }))
    },
    onCharacteristicClick: function (groupName, carId, characteristicName) {
        var vhub = $.connection.gameHub;
        vhub.server.makePick({ GroupName: groupName, CarId: carId, CharacteristicName: characteristicName }).done(function () {
            console.log('Invocation of makePick succeeded');
        }).fail(function (error) {
            console.log('Invocation of makePick failed. Error: ' + error);
        });
        //console.log(characteristicName);
        // $.post('/home/chooseCharacter').done(function (res) {
            // that.setState({ data: data });
        // });
    },
    render: function () {
        var that = this;
        return (
            <div className="card">
                <div className="card__title">{that.props.Title}</div>
                <div className="card__image">
                    {
                        that.props.Result === ""
                        ? ""
                        : <div className="card__win">{that.props.Result}</div>
                    }
                    <img src={that.props.ImageUrl} />
                </div>
                <ul>
                    {
                        that.props.CarCharacteristics.map(function (characteristic) {
                            return (that.props.Active && that.props.Result === "")
                            ?
                                (
                                    <li className={"card__character card__character_active" + (characteristic.IsPicked ? " card__character_picked" : "")} onClick={that.onCharacteristicClick.bind(that, that.props.GroupName, that.props.Id, characteristic.Name)}>
                                        <span className="card__character__name">{characteristic.Name}</span>
                                        <span className="card__character__value">{characteristic.Value}</span>
                                    </li>
                                )
                            :
                                (
                                    <li className={"card__character" + (characteristic.IsPicked ? " card__character_picked" : "")}>
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
