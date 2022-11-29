const express = require("express");
const router = express.Router();
const userController = require("../controllers/userCtl");
const verifyToken = require("../middleware/auth");

// @router PUT /api/user/update
// @Update user profile
// @access private
router.put("/update", verifyToken, userController.updateProfile);

// @router PUT /api/use/changepassword
// @Update user password
// @access private
router.put("/changepassword", verifyToken, userController.updatePassword);

// @router PUT /api/user/changegold
// @Update user gold
// @access private
router.put("/changegold", verifyToken, userController.updateGold);

// @router GET /api/user/profile
// @get user
// @access private
router.get("/profile", verifyToken, userController.get);

module.exports = router;
