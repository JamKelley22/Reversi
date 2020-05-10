import React from "react";
import { Button, Form, InputNumber, Anchor } from "antd";

import "./App.css";
import Reversi from "./reversi/reversi";
const { Link } = Anchor;

const layout = {
  labelCol: { span: 6, offset: 6 }
};
const tailLayout = {
  wrapperCol: {}
};

export default class App extends React.Component {
  state = {
    gameId: -1,
    createGame: false,
    lock: true,
    difficulty: 3
  };

  createNewGame = () => {
    this.setState({
      lock: false
    });
  };

  loadGame = () => {};

  render() {
    const { gameId, createGame, lock } = this.state;

    if (lock) {
      return (
        <div className="App">
          <h1>Reversi</h1>
          <Form
            {...layout}
            name="basic"
            initialValues={{ remember: true }}
            onFinish={this.createNewGame}
            //onFinishFailed={onFinishFailed}
          >
            <Form.Item
              label="Difficulty (1-6)"
              name="difficulty"
              rules={[{ required: true }]}
            >
              <InputNumber
                step={1}
                max={6}
                min={1}
                value={this.state.difficulty}
                onChange={(value: number | undefined) => {
                  if (typeof value === "number") {
                    this.setState({
                      difficulty: value
                    });
                  }
                }}
              />
            </Form.Item>

            {/*
            <Form.Item {...tailLayout}>
              <Button type="primary" htmlType="submit">
                Submit
              </Button>
            </Form.Item> */}

            <Form.Item {...tailLayout}>
              <Button type="primary" htmlType="submit">
                Create New Game
              </Button>
            </Form.Item>
          </Form>
          <a href="https://en.wikipedia.org/wiki/Reversi">
            Learn more about Reversi/Othello
          </a>
        </div>
      );
    } else {
      return (
        <Reversi
          gameId={this.state.gameId}
          difficulty={this.state.difficulty}
        />
      );
    }
  }
}
