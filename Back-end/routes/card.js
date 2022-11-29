const express = require("express");
const router = express.Router();
const cardController = require("../controllers/cardCtl");
const verifyToken = require("../middleware/auth");

// @router POST /api/cards/create
// @Create card
// @access private
router.post("/create", verifyToken, cardController.createCard);

// @router PUT /api/cards/levelup
// @Update card
// @access private
router.put("/levelup", verifyToken, cardController.levelUpCard);

// @router GET /api/cards/allcards
// @Update card
// @access private
router.get("/allcards", verifyToken, cardController.getall);

module.exports = router;
