import * as React from "react";
import HomeContainer from "./HomeContainer";
import CategoryContainer from "./CategoryContainer";
import { TakeAllCuisine } from "../api/Cuisine/cuisineService";
import { useStateValue } from "../context/StateProvider";
import CartContainer from "./CartContainer";
import Loader from "./Loader";

const HomePage = () => {
  const [{ cartShow }] = useStateValue();
  const [loading, setLoading] = React.useState(false);
  const [data, setData] = React.useState([]);
  const [pageTotal, setPageTotal] = React.useState(1);
  const [filter, setFilter] = React.useState({
    page: 0,
    pageSize: 20,
    term: "",
  });

  async function onViewAppearing() {
    setLoading(true);
    let response = await TakeAllCuisine(filter);
    if (response.success) {
      setData(response.data);
      setPageTotal(Math.ceil(response.dataCount / filter.pageSize));
    }
    setLoading(false);
  }

  function onPageChange(e) {
    setFilter({
      ...filter,
      page: e.selected,
    });
  }
  React.useEffect(() => {
    onViewAppearing();
  }, [filter.page, filter.pageSize]);

  return (
    <div className="w-full h-auto flex flex-col items-center justify-center">
      <HomeContainer />

      <section className="w-full my-6">
        <p className="text-2xl font-semibold capitalize text-headingColor relative before:absolute before:rounded-lg before:content before:w-16 before:h-1 before:-bottom-2 before:left-0 before:bg-gradient-to-tr from-orange-400 to-orange-600 transition-all ease-in-out duration-100 mr-auto">
          Danh mục loại hình ăn uống
        </p>
        {loading ? (
          <div className="text-center pt-20">
            <Loader />
          </div>
        ) : (
          <CategoryContainer data={data} />
        )}
      </section>

      {cartShow && <CartContainer />}
    </div>
  );
};

export default HomePage;
