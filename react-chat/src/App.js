import React from "react";
import "./App.css";
import { HubConnectionBuilder } from "@aspnet/signalr";

class App extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      connection: new HubConnectionBuilder()
        .withUrl("http://localhost:50704/chat")
        .build(),
      messages: [],
      message: "",
      userName: ""
    };
  }

  componentDidMount() {
    this.setState(
      {
        message: "message",
        userName: prompt("Enter your name:", "Emre")
      },
      () => {
        this.state.connection
          .start()
          .then(() => {
            console.log("Connection started!");
          })
          .catch(err =>
            console.log("Error while establishing connection :(", err)
          );

        this.state.connection.on("broadcastMessage", (name, message) => {
          let text = { name: name, message: message };
          this.setState({ messages: this.state.messages.concat([text]) });
        });
      }
    );
  }

  handleChange = e => {
    this.setState({ message: e.target.value });
  };

  sendMessage = () => {
    this.state.connection.invoke(
      "send",
      this.state.userName,
      this.state.message
    );
  };

  render() {
    const listItems = this.state.messages.map((item, key) => (
      <li key={key}>
        <strong>{item.name} : </strong>
        {item.message}
      </li>
    ));

    return (
      <div className="container">
        <input type="text" onChange={this.handleChange} />
        <input
          type="button"
          id="sendmessage"
          onClick={this.sendMessage}
          value="Send"
        />
        <ul id="discussion">{listItems}</ul>
      </div>
    );
  }
}
export default App;
