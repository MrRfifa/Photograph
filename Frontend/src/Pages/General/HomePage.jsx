import CardsSection from "../../Components/CardsSection";
import Footer from "../../Layout/Footer";
import Invitation from "../../Components/Invitation";
import Main from "../../Components/Main";
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
