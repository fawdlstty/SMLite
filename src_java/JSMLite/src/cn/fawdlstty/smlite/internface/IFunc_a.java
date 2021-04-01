package cn.fawdlstty.smlite.internface;

public interface IFunc_a<TState extends Enum> {
    public TState call (Object[] args) throws Exception;
}
