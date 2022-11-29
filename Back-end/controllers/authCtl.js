const express = require("express");
const { model } = require("mongoose");
const router = express.Router();
const argon2 = require("argon2");
const jwt = require("jsonwebtoken");

const User = require("../models/User");

const authController = {
  register: async (req, res) => {
    const { username, password, fullName, email, role, gold } = req.body;
    if (!username) {
      return res
        .status(400)
        .json({ success: false, message: "Missing username" });
    }
    if (!password) {
      return res
        .status(400)
        .json({ success: false, message: "Missing password" });
    }
    if (!fullName) {
      return res
        .status(400)
        .json({ success: false, message: "Missing fullName" });
    }
    if (!email) {
      return res.status(400).json({ success: false, message: "Missing email" });
    }
    if (!role) {
      return res.status(400).json({ success: false, message: "Missing role" });
    }
    try {
      //check for username already exists
      const user = await User.findOne({ username });
      if (user) {
        return res
          .status(400)
          .json({ success: false, message: "username already exists" });
      }

      //all good
      const hashesPassword = await argon2.hash(password);
      const newUser = new User({
        username,
        password: hashesPassword,
        fullName,
        email,
        role,
        gold,
      });
      await newUser.save();

      //return token
      const accessToken = jwt.sign(
        { userId: newUser._id },
        process.env.ACCESS_TOKEN_SECRET
      );
      res.json({
        success: true,
        message: "Register successfully",
        accessToken,
      });
    } catch (err) {
      res.status(500).json(err.msg);
    }
  },

  login: async (req, res) => {
    const { username, password } = req.body;

    //validate
    if (!username || !password) {
      return res
        .status(400)
        .json({ success: false, message: "Missing username/password" });
    }
    try {
      //Check for existing user
      const user = await User.findOne({ username });
      if (!user) {
        return res
          .status(400)
          .json({ success: false, message: "Incorect username/password" });
      }

      //Username found
      const passwordValid = await argon2.verify(user.password, password);
      if (!passwordValid) {
        return res
          .status(400)
          .json({ success: false, message: "Incorect username/password" });
      }

      //All good
      //return token
      const accessToken = jwt.sign(
        { userId: user._id },
        process.env.ACCESS_TOKEN_SECRET
      );
      res.json({
        success: true,
        message: "Loggin successfully",
        accessToken,
      });
    } catch (error) {
      res.status(500).json({
        success: false,
        message: "Internal server error",
      });
    }
  },
};

module.exports = authController;
