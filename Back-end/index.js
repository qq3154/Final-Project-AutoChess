const express = require("express");
const mongoose = require("mongoose");
const cors = require("cors");
const dotenv = require("dotenv");

const authRouter = require("./routes/auth");
const matchRouter = require("./routes/match");
const userhRouter = require("./routes/user");
const cardhRouter = require("./routes/card");

const app = express();
app.use(express.json());
app.use(cors());
dotenv.config();

const URL = process.env.DB_URL;
mongoose.connect(
  URL,
  {
    useNewUrlParser: true,
    useUnifiedTopology: true,
  },
  (err) => {
    if (err) throw err;
    console.log("Connected to MongoDB");
  }
);

app.get("/", (req, res) => res.send("hello world"));
app.use("/api/auth", authRouter);
app.use("/api/match", matchRouter);
app.use("/api/user", userhRouter);
app.use("/api/card", cardhRouter);

const PORT = process.env.PORT || 5000;

app.listen(PORT, () => console.log(`Server started at port ${PORT}`));
