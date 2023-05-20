import React, { useState } from "react";
import { FaStar } from "react-icons/fa";
import { Container, Radio, Rating } from "./ratingStyle";
const Rate = (rating) => {
  const [rate, setRate] = useState(rating.rating);

  return (
    <Container>
      {[...Array(5)].map((item, index) => {
        const givenRating = index + 1;
        return (
          <label key={"rating_" + index}>
            <Radio
              type="radio"
              value={givenRating}
              // onClick={() => {
              //   setRate(givenRating);
              // }}
            />
            <Rating>
              <FaStar
                color={
                  givenRating < rate || givenRating === rate
                    ? "#f46a6a"
                    : "rgb(192,192,192)"
                }
              />
            </Rating>
          </label>
        );
      })}
    </Container>
  );
};

export default Rate;
