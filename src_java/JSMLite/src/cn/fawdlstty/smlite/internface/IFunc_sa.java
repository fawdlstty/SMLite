package cn.fawdlstty.smlite.internface;

public interface IFunc_sa<TState extends Enum> {
    public TState call (TState state, Object[] args) throws Exception;
}
