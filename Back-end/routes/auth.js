const express = require("express");
const router = express.Router();
const authController = require("../controllers/authCtl");

// @router POST api/auth/register
// @Register user
// @access public
router.post("/register", authController.register);

// @router POST api/auth/login
// @Login user
// @access public
router.post("/login", authController.login);

module.exports = router;
