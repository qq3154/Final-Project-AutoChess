const express = require("express");
const router = express.Router();
const matchController = require("../controllers/matchCtl");
const verifyToken = require("../middleware/auth");

// @router POST api/matches/create
// @Create match
// @access private
router.post("/create", verifyToken, matchController.create);

// @router GET api/match/matches
// @Get all matches
// @access private
router.get("/create", verifyToken, matchController.getall);

module.exports = router;
