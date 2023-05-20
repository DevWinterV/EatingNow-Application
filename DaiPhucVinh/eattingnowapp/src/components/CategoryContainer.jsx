import React from "react";
import { motion } from "framer-motion";
import { useNavigate } from "react-router-dom";

const CategoryContainer = ({ data }) => {
  const history = useNavigate();
  return (
    <div className="w-full flex items-center gap-3 my-12 scroll-smooth overflow-hidden flex-wrap justify-center">
      {data &&
        data.map((n) => (
          <div
            key={n.CuisineId}
            className="cursor-pointer gap-5 w-275 h-[200px] min-w-[275px] md:w-300 md:min-w-[300px] bg-white border-4 border-orange-100 shadow-2xl rounded-3xl py-2 px-4 my-12 backdrop-blur-lg hover:drop-shadow-lg flex flex-col items-center justify-evenly relative"
          >
            <div
              className="w-full flex items-center justify-center"
              onClick={() => {
                history("/categorypage/" + n.CuisineId, {
                  state: { data: n },
                });
              }}
            >
              <motion.div
                className="w-42 h-42 -mt-10 drop-shadow-2xl"
                whileHover={{ scale: 1.2 }}
              >
                <img
                  src={n.AbsoluteImage}
                  alt=""
                  className="w-full h-full object-contain"
                />
              </motion.div>
            </div>

            <div className="w-full flex flex-col items-center justify-center -mt-8">
              <p className="text-textColor font-semibold text-base md:text-lg">
                {n.Name}
              </p>
            </div>
          </div>
        ))}
    </div>
  );
};

export default CategoryContainer;
