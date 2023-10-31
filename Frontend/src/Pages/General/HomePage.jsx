import CardsSection from "../../Layout/CardsSection";
import Footer from "../../Layout/Footer";
import Invitation from "../../Layout/Invitation";
import Main from "../../Layout/Main";
import Navbar from "../../Layout/Navbar";

const HomePage = () => {
  return (
    <>
      <Navbar />
      <Main />
      <Invitation />
      <CardsSection/>
      <Footer />
    </>
  );
};

export default HomePage;
